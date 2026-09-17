import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { referenceAuthInterceptor } from './reference-auth.interceptor';

describe('referenceAuthInterceptor', () => {
  let http: HttpClient;
  let controller: HttpTestingController;
  let auth: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([referenceAuthInterceptor])),
        provideHttpClientTesting()
      ]
    });
    http = TestBed.inject(HttpClient);
    controller = TestBed.inject(HttpTestingController);
    auth = TestBed.inject(AuthService);
  });

  afterEach(() => controller.verify());

  it('adds the current reference identity and role to API requests', () => {
    auth.setRole('Viewer');
    http.get('/api/projects/1').subscribe();

    const request = controller.expectOne('/api/projects/1');
    expect(request.request.headers.get('X-Reference-User')).toBe('portfolio.user');
    expect(request.request.headers.get('X-Reference-Role')).toBe('Viewer');
    request.flush({});
  });

  it('uses the latest role after the user changes role', () => {
    auth.setRole('Administrator');
    http.get('/api/customers/1').subscribe();

    const request = controller.expectOne('/api/customers/1');
    expect(request.request.headers.get('X-Reference-Role')).toBe('Administrator');
    request.flush({});
  });
});
