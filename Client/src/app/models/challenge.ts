export interface ChallengeCard {
  title: string;
  description: string;
  hint: string;
  testCases: TestCase[];
}

export interface SubmitAnswer {
  isSuccess: boolean;
  errorMessage?: string;
  answer?: string;
  attemptsLeft?: number;
}

export interface TestCase {
  id: string;
  text: string;
  status: 'pending' | 'success' | 'error';
}
