
import { createAction, props } from '@ngrx/store';
import { User } from '../login/user';

export const login = createAction('User Login', props<{ user: User }>());
export const logout = createAction('Logout');

