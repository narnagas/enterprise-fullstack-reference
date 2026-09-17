import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService, ReferenceRole } from './core/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  template: `
    <main class="shell">
      <header class="app-header">
        <div><p class="eyebrow">Enterprise Full-Stack Reference</p><h1>Project Operations</h1><p>Angular client backed by ASP.NET Core APIs for project and customer workflows.</p></div>
        <div class="header-actions">
          <label>Reference role
            <select [value]="auth.role()" (change)="setRole($event)">
              <option value="Viewer">Viewer</option><option value="Editor">Editor</option><option value="Administrator">Administrator</option>
            </select>
          </label>
          <nav aria-label="Application navigation"><a routerLink="/projects" routerLinkActive="active">Projects</a><a routerLink="/customers" routerLinkActive="active">Customers</a></nav>
        </div>
      </header>
      <p class="security-note">Reference authentication is intentionally replaceable. Server policies enforce read versus edit permissions; production deployments should connect an external identity provider.</p>
      <router-outlet />
    </main>
  `,
  styles: [`
    .shell { max-width: 1280px; margin: 0 auto; padding: 32px 24px; } .app-header { display:flex; justify-content:space-between; gap:24px; align-items:end; margin-bottom:12px; } h1 { margin:4px 0 8px; } .app-header p { margin:0; color:#4b5563; } .eyebrow { font-size:12px; font-weight:700; text-transform:uppercase; letter-spacing:.08em; } .header-actions { display:grid; justify-items:end; gap:8px; } label { display:flex; align-items:center; gap:8px; font-size:12px; font-weight:700; color:#4b5563; } select { min-height:34px; border:1px solid #d1d5db; border-radius:6px; background:white; padding:5px 8px; } nav { display:flex; gap:6px; padding:4px; border:1px solid #e5e7eb; border-radius:8px; background:#f9fafb; } nav a { padding:8px 14px; border-radius:6px; color:#374151; text-decoration:none; font-weight:700; font-size:14px; } nav a:hover { background:#f3f4f6; } nav a.active { background:#111827; color:white; } .security-note { margin:0 0 24px; padding:10px 12px; border:1px solid #dbeafe; border-radius:8px; background:#eff6ff; color:#1e3a8a; font-size:12px; } @media (max-width:700px) { .app-header { align-items:start; flex-direction:column; } .header-actions { justify-items:start; } }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {
  constructor(readonly auth: AuthService) {}
  setRole(event: Event): void { this.auth.setRole((event.target as HTMLSelectElement).value as ReferenceRole); }
}
