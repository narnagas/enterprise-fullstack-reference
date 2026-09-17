import { Injectable, signal } from '@angular/core';

export type ReferenceRole = 'Viewer' | 'Editor' | 'Administrator';

@Injectable({ providedIn: 'root' })
export class AuthService {
  readonly userName = signal('portfolio.user');
  readonly role = signal<ReferenceRole>('Editor');

  setRole(role: ReferenceRole): void { this.role.set(role); }
  canEdit(): boolean { return this.role() === 'Editor' || this.role() === 'Administrator'; }
}
