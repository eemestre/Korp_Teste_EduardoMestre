import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Emitir } from './emitir';

describe('Emitir', () => {
  let component: Emitir;
  let fixture: ComponentFixture<Emitir>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Emitir],
    }).compileComponents();

    fixture = TestBed.createComponent(Emitir);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
