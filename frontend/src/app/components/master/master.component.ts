import { Component } from '@angular/core';
import { TestComponent } from '../testComponent/test.component';
import { TesttwoComponent } from '../testtwo/testtwo.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-master',
  standalone: true,
  imports: [TestComponent, TesttwoComponent, CommonModule],
  templateUrl: './master.component.html',
  styleUrl: './master.component.css'
})
export class MasterComponent {
  currentComponent: string = '';

  setCurrentComponent(component: string) {
    this.currentComponent = component;
  }
}
