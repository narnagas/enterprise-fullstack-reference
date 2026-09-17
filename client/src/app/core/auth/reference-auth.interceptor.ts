import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const referenceAuthInterceptor: HttpInterceptorFn = (request, next) => {
  const auth = inject(AuthService);
  return next(request.clone({ setHeaders: {
    'X-Reference-User': auth.userName(),
    'X-Reference-Role': auth.role()
  }}));
};
