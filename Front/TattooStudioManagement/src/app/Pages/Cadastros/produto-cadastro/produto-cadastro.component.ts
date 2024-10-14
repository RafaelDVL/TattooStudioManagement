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
import { ProdutoService } from '../../../Services/produto.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import {MatSnackBar, MatSnackBarModule} from '@angular/material/snack-bar';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { Fornecedor } from '../../../Models/Fornecedor';
import { FornecedorService } from '../../../Services/fornecedor.service';
import { MatSelectModule } from '@angular/material/select';
import { NgxMaskDirective, provideNgxMask} from 'ngx-mask';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-produto-cadastro',
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
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    NgxMaskDirective
  ],
  providers: [provideNgxMask()],
  templateUrl: './produto-cadastro.component.html',
  styleUrls: ['./produto-cadastro.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProdutoCadastroComponent {
  private _snackBar = inject(MatSnackBar);

  isViewMode = false;
  produtoId!: number;
  imageSrc: string | ArrayBuffer | null = null;
  blockButtons: boolean = false;

  constructor(private router: Router, private produtoService: ProdutoService, private fornecedorService: FornecedorService, private toastr: ToastrService, private route: ActivatedRoute,private cdr: ChangeDetectorRef) {}
  
  fornecedores: Fornecedor[] = [];

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.produtoId = Number(params.get('id'));
      const mode = this.route.snapshot.data['mode'];
      this.isViewMode = mode === 'visualizar';

      if (this.produtoId) {
        this.carregarProduto();
      }

      this.carregarFornecedores();
    });
  }

  carregarFornecedores(): void {
    this.fornecedorService.getAll().subscribe({
      next: (dados) => {
        this.fornecedores = dados; // Preenche a lista de fornecedores
      },
      error: (err) => {
        console.error('Erro ao carregar fornecedores', err);
      }
    });
  }

  carregarProduto(): void {
    this.produtoService.getById(this.produtoId).subscribe(produto => {
      console.log(produto);
      this.produtoForm.patchValue(produto);
      if (this.isViewMode) {
        this.produtoForm.disable();
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

  produtoForm = new FormGroup({
    nome: new FormControl('', [Validators.required]),
    descricao: new FormControl('', [Validators.required]),
    preco: new FormControl(0, [Validators.required, Validators.min(0)]),
    quantidadeEmEstoque: new FormControl(0, [Validators.min(0)]),
    fornecedor_codigoId: new FormControl(0, [Validators.required]),
    categoria: new FormControl('', [Validators.required]),
    codigoDeBarras: new FormControl(''),
    estoqueMinimo: new FormControl(0, [Validators.min(0)]),
    dataUltimaAtualizacao: new FormControl(new Date(), [Validators.required]),
    fotos: new FormControl([])
  });

  errorMessage = signal({
    nome: '',
    descricao: '',
    preco: '',
    quantidadeEmEstoque: '',
    fornecedor_codigoId: '',
    categoria: '',
    codigoDeBarras: '',
    estoqueMinimo: '',
    dataUltimaAtualizacao: ''
  });

  imagemSelecionada: File | null = null;
  imagemInvalida = false;

  

  onFileSelected(): void {
    const inputNode: any = document.querySelector('#file');
  
    if (inputNode.files && inputNode.files[0]) {
      const file: File = inputNode.files[0];  // Pega o primeiro arquivo selecionado
  
      // Validação básica do arquivo
      if (file.type.startsWith('image/')) {
        this.imagemSelecionada = file;  // Armazena o arquivo para ser enviado depois
        this.imagemInvalida = false;
  
        // Exemplo de leitura do arquivo (para preview)
        const reader = new FileReader();
        reader.onload = (e: any) => {
          // Atualiza a variável que armazena a URL da imagem para o preview
          this.imageSrc = e.target.result;
          console.log('Imagem carregada para visualização: ', e.target.result);
          
          // Força a detecção de mudanças após atualizar o imageSrc
          this.cdr.detectChanges();
        };
        reader.readAsDataURL(file);  // Lê a imagem como uma URL para pré-visualização
  
        // Resetar o valor do input para garantir que o evento change seja disparado
        inputNode.value = '';
      } else {
        this.imagemInvalida = true;
        this.imageSrc = null;  // Reseta a pré-visualização
        this.errorMessage.update(messages => ({ ...messages, imagemUrl: 'Arquivo inválido. Selecione uma imagem.' }));
      }
    } else {
      // Caso o arquivo não seja selecionado ou o input seja limpo
      this.imageSrc = null;  // Reseta a pré-visualização
    }
  }
  

  updateErrorMessage(campo: string) {
    const control = this.produtoForm.get(campo);
    if (control?.hasError('required')) {
      this.errorMessage.update(messages => ({ ...messages, [campo]: 'Campo obrigatório' }));
    } else {
      this.errorMessage.update(messages => ({ ...messages, [campo]: '' }));
    }
  }

  salvarProduto() {
    if (this.produtoForm.valid) {
      const formData = new FormData();
      formData.append('nome', this.produtoForm.get('nome')?.value ?? '');
      formData.append('descricao', this.produtoForm.get('descricao')?.value ?? '');
      formData.append('preco', this.produtoForm.get('preco')?.value?.toString() ?? '0');
      formData.append('quantidadeEmEstoque', this.produtoForm.get('quantidadeEmEstoque')?.value?.toString() ?? '0');
      formData.append('fornecedor_codigoId', this.produtoForm.get('fornecedor_codigoId')?.value?.toString() ?? '');
      formData.append('categoria', this.produtoForm.get('categoria')?.value ?? '');
      formData.append('codigoDeBarras', this.produtoForm.get('codigoDeBarras')?.value ?? '');
      formData.append('estoqueMinimo', this.produtoForm.get('estoqueMinimo')?.value?.toString() ?? '0');
      formData.append('dataUltimaAtualizacao', this.produtoForm.get('dataUltimaAtualizacao')?.value?.toISOString() ?? '');
  
      if (this.imagemSelecionada) {      
        formData.append('imagem', this.imagemSelecionada); 
      }
  
      if (this.produtoId) {
        // Atualização do produto existente
        this.produtoService.update(this.produtoId, formData).subscribe({
          next: (response) => {
            console.log('Produto atualizado com sucesso', response);
            this.openSnackBarSucess('Produto atualizado com sucesso');
            this.cancelar('produto');
          },
          error: (error) => {
            console.error('Falha ao atualizar o produto: ', error);
            this.openSnackBarError('Falha ao atualizar o item: ' + error);
          }
        });
      } else {
        // Criação de novo produto
        this.produtoService.create(formData).subscribe({
          next: (response) => {
            console.log('Produto criado com sucesso', response);
            this.openSnackBarSucess('Produto criado com sucesso');
            this.cancelar('produto');
          },
          error: (error) => {
            console.error('Falha ao criar o produto: ', error);
            this.openSnackBarError('Falha ao criar o item: ' + error);
          }
        });
      }
    } else {
      console.error('Formulário inválido ou imagem não selecionada');
    }
  }

  cancelar(rota: string) {
    this.produtoForm.reset();
    this.router.navigate([rota]);
  }
}
