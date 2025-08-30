import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { toast, NgxSonnerToaster } from 'ngx-sonner';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgxSonnerToaster],
  standalone: true,
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('daily-regex');
  protected readonly toast = toast;
}
