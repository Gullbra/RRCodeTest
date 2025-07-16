// import { HttpClient } from '@angular/common/http';
// import { Component, OnInit } from '@angular/core';

// interface WeatherForecast {
//   date: string;
//   temperatureC: number;
//   temperatureF: number;
//   summary: string;
// }

// @Component({
//   selector: 'app-root',
//   templateUrl: './app.component.html',
//   styleUrl: './app.component.css'
// })
// export class AppComponent implements OnInit {
//   public forecasts: WeatherForecast[] = [];

//   constructor(private http: HttpClient) {}

//   ngOnInit() {
//     this.getForecasts();
//   }

//   getForecasts() {
//     this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
//       (result) => {
//         this.forecasts = result;
//       },
//       (error) => {
//         console.error(error);
//       }
//     );
//   }

//   title = 'rrcodetest.client';
// }

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { ThemeService } from './services/theme.service';

@Component({
  selector: 'app-root',
  template: `
    <div [attr.data-bs-theme]="themeService.currentTheme()">
      <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container">
          <a class="navbar-brand" href="#" (click)="navigate('/')">
            <i class="fas fa-book me-2"></i>BookManager
          </a>
          
          <div class="navbar-nav ms-auto d-flex flex-row align-items-center">
            <button 
              class="btn btn-outline-light me-3" 
              (click)="themeService.toggleTheme()"
              [title]="themeService.currentTheme() === 'dark' ? 'Switch to Light Mode' : 'Switch to Dark Mode'">
              <i [class]="themeService.currentTheme() === 'dark' ? 'fas fa-sun' : 'fas fa-moon'"></i>
            </button>
            
            <ng-container *ngIf="authService.isAuthenticated(); else loginButtons">
              <span class="navbar-text me-3">Welcome, {{ authService.getCurrentUser()?.email }}</span>
              <button class="btn btn-outline-light" (click)="logout()">
                <i class="fas fa-sign-out-alt me-1"></i>Logout
              </button>
            </ng-container>
            
            <ng-template #loginButtons>
              <button class="btn btn-outline-light me-2" (click)="navigate('/login')">Login</button>
              <button class="btn btn-light" (click)="navigate('/register')">Register</button>
            </ng-template>
          </div>
        </div>
      </nav>
      
      <main class="container mt-4">
        <router-outlet></router-outlet>
      </main>
    </div>
  `
})
export class AppComponent implements OnInit {
  constructor(
    public authService: AuthService,
    public themeService: ThemeService,
    private router: Router
  ) {}

  ngOnInit() {
    this.themeService.initializeTheme();
  }

  navigate(path: string) {
    this.router.navigate([path]);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
