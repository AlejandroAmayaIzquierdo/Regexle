import { User } from './Api';

export interface AuthState {
  isLoggedIn: boolean;
  isGuest: boolean;
  user?: User;
  accessToken?: string;
  refreshToken?: string;
}
