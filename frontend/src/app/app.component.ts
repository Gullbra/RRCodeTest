import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// import { TestComponent } from './components/testComponent/test.component';
import { MasterComponent } from './components/master/master.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MasterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'my-angular-18-project';
}
