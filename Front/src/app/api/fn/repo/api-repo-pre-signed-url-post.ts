/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { OAccount } from '../../models/o-account';

export interface ApiRepoPreSignedUrlPost$Params {
  key: string;
  minExp?: number;
      body?: OAccount
}

export function apiRepoPreSignedUrlPost(http: HttpClient, rootUrl: string, params: ApiRepoPreSignedUrlPost$Params, context?: HttpContext): Observable<StrictHttpResponse<string>> {
  const rb = new RequestBuilder(rootUrl, apiRepoPreSignedUrlPost.PATH, 'post');
  if (params) {
    rb.query('key', params.key, {});
    rb.query('minExp', params.minExp, {});
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<string>;
    })
  );
}

apiRepoPreSignedUrlPost.PATH = '/api/Repo/PreSignedUrl';