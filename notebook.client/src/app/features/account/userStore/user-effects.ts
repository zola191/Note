// import { Injectable } from '@angular/core';
// import { Actions, createEffect, ofType } from '@ngrx/effects';
// import { of } from 'rxjs';
// import { catchError, map, mergeMap } from 'rxjs/operators';
// import { UserService } from '../user.service'; // Импортируйте свой сервис для получения данных пользователя
// import { 
//   loadUserEmail, 
//   loadUserEmailSuccess, 
//   loadUserEmailFailure, 
//   clearUserEmail 
// } from './user.actions';

// @Injectable()
// export class UserEffects {
//   constructor(private actions$: Actions, private userService: UserService) {}

//   loadUserEmail$ = createEffect(() =>
//     this.actions$.pipe(
//       ofType(loadUserEmail),
//       mergeMap(() =>
//         this.userService.getUserEmail().pipe(
//           map(email => loadUserEmailSuccess({ email })),
//           catchError(error => of(loadUserEmailFailure({ error })))
//         )
//       )
//     )
//   );

//   clearUserEmail$ = createEffect(() =>
//     this.actions$.pipe(
//       ofType(clearUserEmail),
//       mergeMap(() =>
//         // Логика очистки, если требуется, например, запрос на сервер или что-то еще
//         of(setUserEmail({ email: null })) // или другой подход в зависимости от логики
//       )
//     )
//   );
// }
