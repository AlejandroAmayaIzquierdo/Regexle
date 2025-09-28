import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { ChallengeCard, SubmitAnswer } from '../models/challenge';
import { firstValueFrom } from 'rxjs';
import { fail, Result, succeed } from '../shared/Util/Result';
import { AuthService } from './auth-service';
import {
  InvalidRegexPattern,
  RegexDoesNotPassAllTestCases,
  SessionExpired,
} from '../shared/Errors';
import { ProblemError } from '../models/Api';

@Injectable({
  providedIn: 'root',
})
export class Challenges {
  private readonly apiUrl = `${environment.apiUrl}/challenges`;
  constructor(private http: HttpClient) {}

  async getDailyChallenge(): Promise<Result<ChallengeCard>> {
    try {
      const challenge = await firstValueFrom(
        this.http.get<ChallengeCard>(`${this.apiUrl}/today`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('ACCESS_TOKEN')}`,
          },
        })
      );
      return succeed(challenge);
    } catch (Error) {
      const error = Error as HttpErrorResponse;

      if (error.status === 401) return fail(SessionExpired);

      return fail({
        code: '',
        message: error.message || 'Error fetching daily challenge',
      });
    }
  }

  public async submitUserInput(input: string): Promise<Result<boolean>> {
    try {
      const result = await firstValueFrom(
        this.http.post<SubmitAnswer>(
          `${this.apiUrl}/submit`,
          { answer: input },
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem('ACCESS_TOKEN')}`,
            },
          }
        )
      );
      return succeed(result.isSuccess);
    } catch (Error) {
      const error = Error as HttpErrorResponse;

      if (error.status === 401) return fail(SessionExpired);

      if (error.status === 422) return fail(RegexDoesNotPassAllTestCases);

      if (error.status === 400) return fail(InvalidRegexPattern);
      return fail({
        code: '',
        message: 'Error submitting user input',
      });
    }
  }
}
