import { User } from './Api';

export interface AuthState {
  isLoggedIn: boolean;
  user?: User;
  accessToken?: string;
  refreshToken?: string;
}
