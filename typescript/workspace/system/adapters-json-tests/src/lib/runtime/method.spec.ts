import { Organization } from '@allors/workspace/default/domain';
import { Pull } from '@allors/workspace/system/domain';
import { Fixture } from '../fixture';
import '../matchers';

let fixture: Fixture;

beforeEach(async () => {
  fixture = new Fixture();
  await fixture.init();
});

test('callSingle', async () => {
  const { workspace, m } = fixture;
  const session = workspace.createSession();

  const pull: Pull = {
    extent: {
      kind: 'Filter',
      objectType: m.Organization,
    },
  };

  let result = await session.pull([pull]);
  const organization = result.collection<Organization>(m.Organization)[0];

  expect(organization.JustDidIt).toBeFalsy();

  const invokeResult = await session.invoke(organization.JustDoIt);

  expect(invokeResult.hasErrors).toBeFalsy();

  result = await session.pull([{ object: organization }]);

  expect(organization.JustDidIt).toBeTruthy();
  expect(organization.JustDidItDerived).toBeTruthy();
});

test('callMultiple', async () => {
  const { workspace, m } = fixture;
  const session = workspace.createSession();

  const pull: Pull = {
    extent: {
      kind: 'Filter',
      objectType: m.Organization,
    },
  };

  let result = await session.pull([pull]);
  const organization1 = result.collection<Organization>(m.Organization)[0];
  const organization2 = result.collection<Organization>(m.Organization)[1];

  expect(organization1.JustDidIt).toBeFalsy();

  const invokeResult = await session.invoke([
    organization1.JustDoIt,
    organization2.JustDoIt,
  ]);

  expect(invokeResult.hasErrors).toBeFalsy();

  result = await session.pull([pull]);

  expect(organization1.JustDidIt).toBeTruthy();
  expect(organization1.JustDidItDerived).toBeTruthy();

  expect(organization2.JustDidIt).toBeTruthy();
  expect(organization2.JustDidItDerived).toBeTruthy();
});

test('callMultipleIsolated', async () => {
  const { workspace, m } = fixture;
  const session = workspace.createSession();

  const pull: Pull = {
    extent: {
      kind: 'Filter',
      objectType: m.Organization,
    },
  };

  let result = await session.pull([pull]);
  const organization1 = result.collection<Organization>(m.Organization)[0];
  const organization2 = result.collection<Organization>(m.Organization)[1];

  expect(organization1.JustDidIt).toBeFalsy();

  const invokeResult = await session.invoke(
    [organization1.JustDoIt, organization2.JustDoIt],
    { isolated: true }
  );

  expect(invokeResult.hasErrors).toBeFalsy();

  result = await session.pull([pull]);

  expect(organization1.JustDidIt).toBeTruthy();
  expect(organization1.JustDidItDerived).toBeTruthy();

  expect(organization2.JustDidIt).toBeTruthy();
  expect(organization2.JustDidItDerived).toBeTruthy();
});
