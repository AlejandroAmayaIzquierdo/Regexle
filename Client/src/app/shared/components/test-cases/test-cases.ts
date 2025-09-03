import { Component, Input } from '@angular/core';
import { TestCase } from '../../../models/challenge';

@Component({
  selector: 'test-cases',
  imports: [],
  templateUrl: './test-cases.html',
  styleUrl: './test-cases.css',
})
export class TestCases {
  @Input({ required: false }) public testCases: TestCase[] = [];

  getColorStatus(testCase: TestCase): string {
    switch (testCase.status) {
      case 'success':
        return 'bg-green-100 text-green-800';
      case 'error':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-slate-100 text-slate-800';
    }
  }
}
