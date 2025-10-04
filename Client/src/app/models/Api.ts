import { JwtPayload } from 'jwt-decode';

export interface ProblemError {
  type: string;
  title: string;
  status: number;
  detail: string;
}

export interface User {
  id: string;
  email: string;
  userName: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  user: User;
}

export interface MyClaims extends JwtPayload {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
  permissions: string;
}
