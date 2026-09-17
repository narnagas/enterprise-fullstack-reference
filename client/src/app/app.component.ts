import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CustomerSearchComponent } from './features/customers/customer-search.component';
import { ProjectSearchComponent } from './features/projects/project-search.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProjectSearchComponent, CustomerSearchComponent],
  template: `
    <main class="shell">
      <header>
        <p class="eyebrow">Enterprise Full-Stack Reference</p>
        <h1>Project Operations</h1>
        <p>Angular client backed by ASP.NET Core APIs for project and customer workflows.</p>
      </header>
      <app-project-search />
      <app-customer-search />
    </main>
  `,
  styles: [`
    .shell { max-width: 1280px; margin: 0 auto; padding: 32px 24px; }
    header { margin-bottom: 24px; }
    h1 { margin: 4px 0 8px; }
    header p { margin: 0; color: #4b5563; }
    .eyebrow { font-size: 12px; font-weight: 700; text-transform: uppercase; letter-spacing: .08em; }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {}
