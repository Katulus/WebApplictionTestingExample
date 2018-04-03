import { HttpClient, HttpErrorResponse } from '@angular/common/http';

export abstract class HttpServiceBase {
  constructor(private http: HttpClient, private baseUrl: string) {
  }

  protected get<T>(path: string, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
    this.http.get<T>(this.baseUrl + path).subscribe((result: T) => {
      callback(result);
    }, (error: HttpErrorResponse) => {
      if (errorCallback) {
        errorCallback(error.statusText);
      }
    });
  }

  protected post<T>(path: string, data: any, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
    this.http.post(this.baseUrl + path, data).subscribe((result: T) => {
      callback(result);
    }, (error: HttpErrorResponse) => {
      if (errorCallback) {
        errorCallback(error.statusText);
      }
    });
  }
}
