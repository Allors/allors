import { DerivationRole } from './derivation-role';

export interface ResponseDerivationError {
  /** Message */
  m: string;

  /** Roles */
  r: DerivationRole[];
}
