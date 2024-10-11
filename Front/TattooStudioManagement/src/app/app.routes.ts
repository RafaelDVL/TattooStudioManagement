import { Routes } from '@angular/router';
import { ProdutoListaComponent } from './Pages/Listas/produto-lista/produto-lista.component';
import { FornecedorListaComponent } from './Pages/Listas/fornecedor-lista/fornecedor-lista.component';
import { HomeComponent } from './Pages/home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'produto', component: ProdutoListaComponent },
  { path: 'fornecedor', component: FornecedorListaComponent },
];
