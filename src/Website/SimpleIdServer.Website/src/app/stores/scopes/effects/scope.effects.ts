import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { completeGetAll, completeSearch, errorGetAll, errorSearch, startGetAll, startSearch } from '../actions/scope.actions';
import { OAuthScopeService } from '../services/scope.service';

@Injectable()
export class OAuthScopeEffects {
  constructor(
    private actions$: Actions,
    private oauthScopeService: OAuthScopeService,
  ) { }

  @Effect()
  searchOAuthScopes$ = this.actions$
    .pipe(
      ofType(startSearch),
      mergeMap((evt) => {
        return this.oauthScopeService.search(evt.startIndex, evt.count, evt.order, evt.direction)
          .pipe(
            map(content => completeSearch({ content: content })),
            catchError(() => of(errorSearch()))
          );
      }
      )
  );

  @Effect()
  getAllOAuthScopes$ = this.actions$
    .pipe(
      ofType(startGetAll),
      mergeMap((evt) => {
        return this.oauthScopeService.getAll()
          .pipe(
            map(content => completeGetAll({ content: content })),
            catchError(() => of(errorGetAll()))
          );
      }
      )
    );
}