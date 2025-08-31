import { Error } from './Util/Result';

export const SessionExpired: Error = {
  code: 'SessionExpired',
  message: 'Your session has expired. Please log in again.',
};
