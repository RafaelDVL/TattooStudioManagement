import { Routes } from '@angular/router';
import { ProdutoListaComponent } from './Pages/Listas/produto-lista/produto-lista.component';
import { FornecedorListaComponent } from './Pages/Listas/fornecedor-lista/fornecedor-lista.component';
import { HomeComponent } from './Pages/home/home.component';
import { FornecedorCadastroComponent } from './Pages/Cadastros/fornecedor-cadastro/fornecedor-cadastro.component';
import { ProdutoCadastroComponent } from './Pages/Cadastros/produto-cadastro/produto-cadastro.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'fornecedor', component: FornecedorListaComponent },
  { path: 'fornecedor/cadastro', component: FornecedorCadastroComponent },
  { path: 'fornecedor/visualizar/:id', component: FornecedorCadastroComponent, data: { mode: 'visualizar' } },
  { path: 'fornecedor/editar/:id', component: FornecedorCadastroComponent, data: { mode: 'editar' } },
  { path: 'produto', component: ProdutoListaComponent },
  { path: 'produto/cadastro', component: ProdutoCadastroComponent },
  { path: 'produto/visualizar/:id', component: ProdutoCadastroComponent, data: { mode: 'visualizar' } },
  { path: 'produto/editar/:id', component: ProdutoCadastroComponent, data: { mode: 'editar' } },
];
