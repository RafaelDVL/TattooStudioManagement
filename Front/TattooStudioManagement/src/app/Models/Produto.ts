import { Fornecedor } from './Fornecedor';

export class Produto {
  public id: number = 0;
  public fornecedor_codigoId: number = 0;
  public nome: string = '';
  public descricao: string = '';
  public preco: number = 0;
  public quantidadeEmEstoque?: number;
  public dataUltimaAtualizacao: Date = new Date();
  public categoria: string = '';
  public codigoDeBarras?: string;
  public estoqueMinimo: number = 0;
  public foto: string = "" ;
  public fornecedor?: Fornecedor;
}
