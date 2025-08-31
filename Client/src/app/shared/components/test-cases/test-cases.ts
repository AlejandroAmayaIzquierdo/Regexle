import { Component, Input } from '@angular/core';
import { TestCase } from '../../../models/challenge';

@Component({
  selector: 'app-test-cases',
  imports: [],
  templateUrl: './test-cases.html',
  styleUrl: './test-cases.css',
})
export class TestCases {
  @Input() public testCases: TestCase[] = [];
}
