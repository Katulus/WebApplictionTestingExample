import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export abstract class HttpServiceBase {
  private _baseAddress: string = 'http://localhost:63598';

  constructor(private http: HttpClient) {
  }

  protected get<T>(path: string, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
    this.http.get<T>(this._baseAddress + path).subscribe((result: T) => {
      callback(result);
    }, error => {
      if (errorCallback) {
        errorCallback(error);
      }
    });
  }

  protected post<T>(path: string, data: any, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
    this.http.post(this._baseAddress + path, data).subscribe((result: T) => {
      callback(result);
    }, error => {
      if (errorCallback) {
        errorCallback(error);
      }
    });
  }
}
