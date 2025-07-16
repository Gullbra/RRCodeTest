import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, CreateBookRequest } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private readonly API_URL = 'http://localhost:4200/api'; // Adjust to your backend URL

  constructor(private http: HttpClient) {}

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.API_URL}/books`);
  }

  getBook(id: string): Observable<Book> {
    return this.http.get<Book>(`${this.API_URL}/books/${id}`);
  }

  createBook(book: CreateBookRequest): Observable<Book> {
    return this.http.post<Book>(`${this.API_URL}/books`, book);
  }

  updateBook(id: string, book: Partial<CreateBookRequest>): Observable<Book> {
    return this.http.put<Book>(`${this.API_URL}/books/${id}`, book);
  }

  deleteBook(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/books/${id}`);
  }
}
