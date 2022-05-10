import { createReducer, on } from '@ngrx/store';

import { login, logout } from './auth.actions';
import { User } from '../login/user';

export interface AuthState {
  user: User | null;
}

export const initialState: AuthState = {
  user: null
};


export const authReducer = createReducer(

  initialState,

  on(login, (state, action) => {
    return {
      user: action.user
    }
  }),

  on(logout, (state, action) => {
    return {
      user: null
    }
  })

);

