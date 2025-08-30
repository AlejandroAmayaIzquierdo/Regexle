import { Component, OnInit, signal } from '@angular/core';
import { ChallengeCardComponent } from '../shared/components/challenge-card/challenge-card';
import { Challenges } from '../services/challenges';
import { ChallengeCard } from '../models/challenge';
import { toast } from 'ngx-sonner';

@Component({
  selector: 'app-home',
  imports: [ChallengeCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  constructor(public challenges: Challenges) {}

  loadingChallenge = signal(false);
  loadingSubmit = signal(false);

  challenge = signal<ChallengeCard | undefined>(undefined);

  ngOnInit() {
    this.getDailyChallenge();
  }

  public async getDailyChallenge() {
    this.loadingChallenge.set(true);
    const challenge = await this.challenges.getDailyChallenge();
    this.challenge.set(challenge);
    this.loadingChallenge.set(false);
  }

  public async submitUserInput(input: string) {
    this.loadingSubmit.set(true);
    const result = await this.challenges.submitUserInput(input);

    if (result) {
      toast.success('Input submitted successfully!');
    } else {
      toast.error('Failed to submit input.');
    }
    this.loadingSubmit.set(false);
  }
}
