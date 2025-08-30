export interface ChallengeCard {
  title: string;
  description: string;
  hint: string;
}

export interface SubmitAnswer {
  isSuccess: boolean;
  errorMessage?: string;
  answer?: string;
  attemptsLeft?: number;
}
