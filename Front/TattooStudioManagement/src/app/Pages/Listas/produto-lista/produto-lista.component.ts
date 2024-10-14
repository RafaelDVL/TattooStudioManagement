import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import {MatCardModule} from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Produto } from '../../../Models/Produto';
import { Router, RouterLink } from '@angular/router';
import { ProdutoService } from '../../../Services/produto.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-produto-lista',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    RouterLink,
    MatSnackBarModule
  ],
  templateUrl: './produto-lista.component.html',
  styleUrl: './produto-lista.component.scss'
})
export class ProdutoListaComponent implements AfterViewInit {
  produtoes: Produto[] = [];
  private _snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['id','action', 'imagemUrl', 'nome', 'linkUltimaCompra'];
  dataSource: MatTableDataSource<Produto>;


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private router: Router, private produtoService: ProdutoService) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.carregarProdutoes();
  }

  carregarProdutoes(): void {
    this.produtoService.getAll().subscribe({
      next: (dados) => {
        this.produtoes = dados;
        this.dataSource.data = this.produtoes;  // Atualiza o MatTableDataSource com os dados recebidos
        this.dataSource.paginator = this.paginator;  // Reaplica o paginator após os dados serem carregados
        this.dataSource.sort = this.sort;
      },
      error: (err) => {
        console.error('Erro ao carregar produtoes', err);
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

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  redirection(rota:string){
    this.router.navigate([rota]);
  }


  visualizar(row: Produto) {
    this.router.navigate([`/produto/visualizar/${row.id}`]);
  }
  
  editar(row: Produto) {
    this.router.navigate([`/produto/editar/${row.id}`]);
  }

  apagar(row: Produto) {
    // Mostrar Snackbar de confirmação
    const snackBarRef = this._snackBar.open('Tem certeza que deseja apagar este produto?', 'Sim', {
      duration: 4000,  // Tempo de exibição do SnackBar
      horizontalPosition: 'center',
      verticalPosition: 'top',
    });

    // Aguardar até o usuário clicar em "Sim"
    snackBarRef.onAction().subscribe(() => {
      // Somente após o clique no botão "Sim", a exclusão será realizada
      this.produtoService.delete(row.id).subscribe({
        next: () => {
          this.carregarProdutoes();
          this.openSnackBarSucess('Produto apagado com sucesso');
        },
        error: (error) => {
          console.error('Falha ao apagar o produto: ', error);
          this.openSnackBarError('Falha ao apagar o item: ' + error);
        }
      });
    });
  }
}
