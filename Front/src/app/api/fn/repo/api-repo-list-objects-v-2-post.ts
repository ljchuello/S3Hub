/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { OAccount } from '../../models/o-account';
import { OFile } from '../../models/o-file';

export interface ApiRepoListObjectsV2Post$Params {
  directory: string;
  getInfo?: boolean;
      body?: OAccount
}

export function apiRepoListObjectsV2Post(http: HttpClient, rootUrl: string, params: ApiRepoListObjectsV2Post$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<OFile>>> {
  const rb = new RequestBuilder(rootUrl, apiRepoListObjectsV2Post.PATH, 'post');
  if (params) {
    rb.query('directory', params.directory, {});
    rb.query('getInfo', params.getInfo, {});
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<OFile>>;
    })
  );
}

apiRepoListObjectsV2Post.PATH = '/api/Repo/ListObjectsV2';
