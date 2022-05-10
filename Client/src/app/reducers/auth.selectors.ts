import {createFeatureSelector, createSelector} from '@ngrx/store';
import {AuthState} from './auth.reducer';


export const selectAuthState =
    createFeatureSelector<AuthState>("auth");


export const isLoggedIn = createSelector(
    selectAuthState,
    auth =>  !!auth.user

);

export const currentUser = createSelector(
    selectAuthState,
    auth => {
        if(auth.user){
            return auth.user
        } else {
            return null;
        }
    }
);