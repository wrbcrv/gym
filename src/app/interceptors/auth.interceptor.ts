import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const withCredReq = req.clone({
    withCredentials: true
  });
  return next(withCredReq);
};
