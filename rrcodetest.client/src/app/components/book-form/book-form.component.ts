import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.model';

@Component({
  selector: 'app-book-form',
  template: `
    <div class="row justify-content-center">
      <div class="col-md-8">
        <div class="card">
          <div class="card-header">
            <h2 class="mb-0">
              <i [class]="isEditing ? 'fas fa-edit' : 'fas fa-plus'" class="me-2"></i>
              {{ isEditing ? 'Edit Book' : 'Add New Book' }}
            </h2>
          </div>
          <div class="card-body">
            <form [formGroup]="bookForm" (ngSubmit)="onSubmit()">
              <div class="row">
                <div class="col-md-6">
                  <div class="mb-3">
                    <label class="form-label">Title *</label>
                    <input 
                      type="text" 
                      class="form-control"
                      formControlName="title"
                      [class.is-invalid]="bookForm.get('title')?.invalid && bookForm.get('title')?.touched">
                    <div class="invalid-feedback" *ngIf="bookForm.get('title')?.invalid && bookForm.get('title')?.touched">
                      Title is required
                    </div>
                  </div>
                </div>
                
                <div class="col-md-6">
                  <div class="mb-3">
                    <label class="form-label">Author *</label>
                    <input 
                      type="text" 
                      class="form-control"
                      formControlName="author"
                      [class.is-invalid]="bookForm.get('author')?.invalid && bookForm.get('author')?.touched">
                    <div class="invalid-feedback" *ngIf="bookForm.get('author')?.invalid && bookForm.get('author')?.touched">
                      Author is required
                    </div>
                  </div>
                </div>
              </div>
              
              <div class="row">
                <div class="col-md-6">
                  <div class="mb-3">
                    <label class="form-label">Genre *</label>
                    <select 
                      class="form-select"
                      formControlName="genre"
                      [class.is-invalid]="bookForm.get('genre')?.invalid && bookForm.get('genre')?.touched">
                      <option value="">Select a genre</option>
                      <option value="Fiction">Fiction</option>
                      <option value="Non-Fiction">Non-Fiction</option>
                      <option value="Mystery">Mystery</option>
                      <option value="Romance">Romance</option>
                      <option value="Science Fiction">Science Fiction</option>
                      <option value="Fantasy">Fantasy</option>
                      <option value="Biography">Biography</option>
                      <option value="History">History</option>
                      <option value="Self-Help">Self-Help</option>
                      <option value="Other">Other</option>
                    </select>
                    <div class="invalid-feedback" *ngIf="bookForm.get('genre')?.invalid && bookForm.get('genre')?.touched">
                      Please select a genre
                    </div>
                  </div>
                </div>
                
                <div class="col-md-6">
                  <div class="mb-3">
                    <label class="form-label">Publication Year *</label>
                    <input 
                      type="number" 
                      class="form-control"
                      formControlName="publicationYear"
                      min="1000"
                      max="2025"
                      [class.is-invalid]="bookForm.get('publicationYear')?.invalid && bookForm.get('publicationYear')?.touched">
                    <div class="invalid-feedback" *ngIf="bookForm.get('publicationYear')?.invalid && bookForm.get('publicationYear')?.touched">
                      Please enter a valid publication year
                    </div>
                  </div>
                </div>
              </div>
              
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea 
                  class="form-control"
                  formControlName="description"
                  rows="4"
                  placeholder="Brief description of the book..."></textarea>
              </div>
              
              <div class="alert alert-danger" *ngIf="errorMessage">
                {{ errorMessage }}
              </div>
              
              <div class="d-flex justify-content-between">
                <button 
                  type="button" 
                  class="btn btn-secondary"
                  (click)="router.navigate(['/'])">
                  <i class="fas fa-arrow-left me-2"></i>Cancel
                </button>
                <button 
                  type="submit" 
                  class="btn btn-primary"
                  [disabled]="bookForm.invalid || loading">
                  <span *ngIf="loading" class="spinner-border spinner-border-sm me-2"></span>
                  <i *ngIf="!loading" [class]="isEditing ? 'fas fa-save' : 'fas fa-plus'" class="me-2"></i>
                  {{ loading ? 'Saving...' : (isEditing ? 'Update Book' : 'Add Book') }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  `
})
export class BookFormComponent implements OnInit {
  bookForm: FormGroup;
  isEditing = false;
  bookId: string | null = null;
  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private route: ActivatedRoute,
    public router: Router
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      genre: ['', Validators.required],
      publicationYear: ['', [Validators.required, Validators.min(1000), Validators.max(2025)]],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.bookId = this.route.snapshot.paramMap.get('id');
    this.isEditing = !!this.bookId;
    
    if (this.isEditing && this.bookId) {
      this.loadBook();
    }
  }

  loadBook(): void {
    this.loading = true;
    this.bookService.getBook(this.bookId!).subscribe({
      next: (book: Book) => {
        this.bookForm.patchValue(book);
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'Error loading book: ' + error;
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.bookForm.valid) {
      this.loading = true;
      this.errorMessage = '';
      
      const bookData = this.bookForm.value;
      
      const request = this.isEditing 
        ? this.bookService.updateBook(this.bookId!, bookData)
        : this.bookService.createBook(bookData);
        
      request.subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.errorMessage = error;
          this.loading = false;
        }
      });
    }
  }
}