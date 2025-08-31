export type Failure = {
  _tag: 'Failure';
  success: false;
  error: Error;
  data?: never;
};

export type Success<T = void> = {
  _tag: 'Success';
  success: true;
  data: T;
};

export type Error = {
  code: string;
  message: string;
};

export type Result<T = void, E = unknown> = Failure | Success<T>;

export function succeed<T = void>(data: T): Success<T> {
  return {
    _tag: 'Success',
    data: data,
    success: true,
  };
}

export function fail(error: Error): Failure {
  return {
    _tag: 'Failure',
    error: error,
    success: false,
  };
}
