/* eslint-disable */
export default {
  displayName: 'workspace-system-adapters-tests',
  preset: '../../../jest.preset.js',
  testEnvironment: 'node',
  transform: {
    '^.+\\.[tj]s$': ['ts-jest', { tsconfig: '<rootDir>/tsconfig.spec.json' }],
  },
  moduleFileExtensions: ['ts', 'js', 'html'],
  coverageDirectory: '../../../coverage/workspace/system/adapters-tests',
  // Allors
  reporters: [
    'default',
    [
      'jest-trx-results-processor',
      {
        outputFile:
          '../artifacts/tests/typscript.workspace-adapters-system.trx',
      },
    ],
  ],
  maxWorkers: 1,
  testTimeout: 60000,
};
