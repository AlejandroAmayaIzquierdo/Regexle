import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChallengeCard } from '../../../models/challenge';
import { LoaderCircle, LucideAngularModule } from 'lucide-angular';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'challenge-card',
  imports: [FormsModule, LucideAngularModule],
  templateUrl: './challenge-card.html',
})
export class ChallengeCardComponent {
  public constructor(public authService: AuthService) {}
  readonly LoadingIcon = LoaderCircle;
  @Input({ required: false }) public loading: boolean = false;
  @Input({ required: false }) public loadingSubmit: boolean = false;

  @Input({ required: false }) public challenge?: ChallengeCard | undefined;

  userInput: string = '';

  @Output() userInputSubmit = new EventEmitter<string>();

  get isComplete(): boolean {
    return this.userInput !== '';
  }
}
