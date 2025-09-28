import { Error } from './Util/Result';

export const SessionExpired: Error = {
  code: 'SessionExpired',
  message: 'Your session has expired. Please log in again.',
};

export const RegexDoesNotPassAllTestCases: Error = {
  code: 'RegexDoesNotPassAllTestCases',
  message: 'The provided regex does not pass all test cases.',
};

export const InvalidRegexPattern: Error = {
  code: 'InvalidRegexPattern',
  message: 'The provided regex pattern is invalid.',
};

export const RegexNotFound: Error = {
  code: 'RegexNotFound',
  message: 'The provided regex pattern was not found.',
};
