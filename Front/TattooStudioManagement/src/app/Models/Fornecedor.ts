import { Produto } from "./Produto";

export class Fornecedor {
    id: number = 0;
    nome: string = "";
    linkUltimaCompra?: string = "" ;
    imagemUrl?: string = "" ;
    produtos?: Produto[];
  }