import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpErrorResponse} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Fornecedor } from '../Models/Fornecedor';

@Injectable({
  providedIn: 'root'
})
export class FornecedorService {

  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getAll(): Observable<Fornecedor[]>{
    return this.http.get<Fornecedor[]>(`${this.apiUrl}/fornecedor`)
      .pipe(
        catchError(this.handleError)  // Tratamento de erros
      );
  }

  getById(cod:number): Observable<Fornecedor>{
    return this.http.get<Fornecedor>(`${this.apiUrl}/fornecedor/${cod}`)
      .pipe(
        catchError(this.handleError)  // Tratamento de erros
      );
  }


  create(formData: FormData):Observable<any>{
    return this.http.post(`${this.apiUrl}/fornecedor`, formData)
    .pipe(
      catchError(this.handleError)
    );
  }

  update(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.apiUrl}/fornecedor/${id}`, formData)
      .pipe(
        catchError(this.handleError)
      );
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/fornecedor/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // Erro do lado do cliente
      console.error('Ocorreu um erro:', error.error.message);
    } else {
      // Erro do lado do servidor
      console.error(
        `Backend retornou código ${error.status}, ` +
        `corpo foi: ${error.error}`);
    }
    // Retorna um erro customizado para a aplicação
    return throwError(() => new Error('Ocorreu um erro inesperado. Tente novamente mais tarde.'));
  }
}
