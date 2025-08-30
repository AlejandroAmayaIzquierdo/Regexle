import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { ChallengeCard, SubmitAnswer } from '../models/challenge';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Challenges {
  private readonly apiUrl = `${environment.apiUrl}/challenges`;
  constructor(private http: HttpClient) {}

  async getDailyChallenge(): Promise<ChallengeCard | undefined> {
    try {
      return await firstValueFrom(
        this.http.get<ChallengeCard>(`${this.apiUrl}/today`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('ACCESS_TOKEN')}`,
          },
        })
      );
    } catch (Error) {
      console.error('Error fetching daily challenge:', Error);
      return undefined;
    }
  }

  public async submitUserInput(input: string): Promise<boolean> {
    try {
      const result = await firstValueFrom(
        this.http.post<SubmitAnswer>(`${this.apiUrl}/submit`, { answer: input })
      );
      return result.isSuccess;
    } catch (Error) {
      console.error('Error submitting user input:', Error);
      return false;
    }
  }
}
