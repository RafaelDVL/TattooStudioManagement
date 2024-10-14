import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common'; // Importando CommonModule
import { merge } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { FornecedorService } from '../../../Services/fornecedor.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import {MatSnackBar, MatSnackBarModule} from '@angular/material/snack-bar';

@Component({
  selector: 'app-fornecedor-cadastro',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ToastrModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './fornecedor-cadastro.component.html',
  styleUrls: ['./fornecedor-cadastro.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FornecedorCadastroComponent {

  private _snackBar = inject(MatSnackBar);

  isViewMode = false;
  fornecedorId!: number;
  blockButtons: boolean = false;

  constructor(private router: Router, private fornecedorService: FornecedorService, private toastr: ToastrService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.fornecedorId = Number(params.get('id'));
      const mode = this.route.snapshot.data['mode'];
      this.isViewMode = mode === 'visualizar';

      if (this.fornecedorId) {
        this.carregarFornecedor();
      }
    });
  }

  carregarFornecedor(): void {
    this.fornecedorService.getById(this.fornecedorId).subscribe(fornecedor => {
      console.log(fornecedor);
      this.fornecedorForm.patchValue(fornecedor);
      if (this.isViewMode) {
        this.fornecedorForm.disable();
        this.blockButtons = true;  // Desativa o formulário no modo de visualização
      }
    });
  }

  openSnackBarSucess(message: string) {
    this._snackBar.open(message, "Fechar", {
      duration: 3000,
      horizontalPosition: "end",
      verticalPosition: "top",
      panelClass: ['snackbar-success']  // Classe personalizada para sucesso
    });
  }
  
  openSnackBarError(message: string) {
    this._snackBar.open(message, "Fechar", {
      duration: 3000,
      horizontalPosition: "end",
      verticalPosition: "top",
      panelClass: ['snackbar-error']  // Classe personalizada para erro
    });
  }

  fornecedorForm = new FormGroup({
    nome: new FormControl('', [Validators.required]),
    linkUltimaCompra: new FormControl(''),
    imagemUrl: new FormControl(''),
  });

  errorMessage = signal({
    nome: '',
    linkUltimaCompra: '',
    imagemUrl: ''
  });

  imagemSelecionada: File | null = null;
  imagemInvalida = false;

  

  onFileSelected() {
    const inputNode: any = document.querySelector('#file');
    
    if (inputNode.files && inputNode.files[0]) {
      const file: File = inputNode.files[0];  // Pega o primeiro arquivo selecionado
      
      // Validação básica do arquivo (opcional)
      if (file.type.startsWith('image/')) {
        this.imagemSelecionada = file;  // Armazena o arquivo para ser enviado depois
        this.imagemInvalida = false;
  
        // Exemplo de leitura do arquivo (para preview, se necessário)
        const reader = new FileReader();
        reader.onload = (e: any) => {
          console.log('Imagem carregada para visualização (opcional): ', e.target.result);
        };
        reader.readAsDataURL(file);  // Lê a imagem como uma URL para pré-visualização (opcional)
  
      } else {
        this.imagemInvalida = true;
        this.errorMessage.update(messages => ({ ...messages, imagemUrl: 'Arquivo inválido. Selecione uma imagem.' }));
      }
    }
  }

  updateErrorMessage(campo: string) {
    const control = this.fornecedorForm.get(campo);
    if (control?.hasError('required')) {
      this.errorMessage.update(messages => ({ ...messages, [campo]: 'Campo obrigatório' }));
    } else {
      this.errorMessage.update(messages => ({ ...messages, [campo]: '' }));
    }
  }

  salvarFornecedor() {
    if (this.fornecedorForm.valid) {
      const formData = new FormData();
      formData.append('nome', this.fornecedorForm.get('nome')?.value ?? '');
      formData.append('linkUltimaCompra', this.fornecedorForm.get('linkUltimaCompra')?.value ?? '');
      formData.append('imagemUrl', this.fornecedorForm.get('imagemUrl')?.value ?? '');
 
      if (this.imagemSelecionada) {      
        formData.append('imagem', this.imagemSelecionada); 
      }
  
      if (this.fornecedorId) {
        // Atualização do fornecedor existente
        this.fornecedorService.update(this.fornecedorId, formData).subscribe({
          next: (response) => {
            console.log('Fornecedor atualizado com sucesso', response);
            this.openSnackBarSucess('Fornecedor atualizado com sucesso');
            this.cancelar('fornecedor');
          },
          error: (error) => {
            console.error('Falha ao atualizar o fornecedor: ', error);
            this.openSnackBarError('Falha ao atualizar o item: ' + error);
          }
        });
      } else {
        // Criação de novo fornecedor
        this.fornecedorService.create(formData).subscribe({
          next: (response) => {
            console.log('Fornecedor criado com sucesso', response);
            this.openSnackBarSucess('Fornecedor criado com sucesso');
            this.cancelar('fornecedor');
          },
          error: (error) => {
            console.error('Falha ao criar o fornecedor: ', error);
            this.openSnackBarError('Falha ao criar o item: ' + error);
          }
        });
      }
    } else {
      console.error('Formulário inválido ou imagem não selecionada');
    }
  }

  cancelar(rota: string) {
    this.fornecedorForm.reset();
    this.router.navigate([rota]);
  }
}
