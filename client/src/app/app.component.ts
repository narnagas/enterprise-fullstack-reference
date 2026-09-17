import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  template: `
    <main class="shell">
      <header class="app-header">
        <div>
          <p class="eyebrow">Enterprise Full-Stack Reference</p>
          <h1>Project Operations</h1>
          <p>Angular client backed by ASP.NET Core APIs for project and customer workflows.</p>
        </div>
        <nav aria-label="Application navigation">
          <a routerLink="/projects" routerLinkActive="active">Projects</a>
          <a routerLink="/customers" routerLinkActive="active">Customers</a>
        </nav>
      </header>
      <router-outlet />
    </main>
  `,
  styles: [`
    .shell { max-width: 1280px; margin: 0 auto; padding: 32px 24px; }
    .app-header { display: flex; justify-content: space-between; gap: 24px; align-items: end; margin-bottom: 24px; }
    h1 { margin: 4px 0 8px; }
    .app-header p { margin: 0; color: #4b5563; }
    .eyebrow { font-size: 12px; font-weight: 700; text-transform: uppercase; letter-spacing: .08em; }
    nav { display: flex; gap: 6px; padding: 4px; border: 1px solid #e5e7eb; border-radius: 8px; background: #f9fafb; }
    nav a { padding: 8px 14px; border-radius: 6px; color: #374151; text-decoration: none; font-weight: 700; font-size: 14px; }
    nav a:hover { background: #f3f4f6; }
    nav a.active { background: #111827; color: white; }
    @media (max-width: 700px) { .app-header { align-items: start; flex-direction: column; } }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {}
