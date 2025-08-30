export interface ProblemError {
  type: string;
  title: string;
  status: number;
  detail: string;
}

export interface User {
  id: string;
  email: string;
  name: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  user: User;
}
