create table USERS (
  id uuid primary key default gen_random_uuid(),
  created_at timestamp with time zone default now() not null,
  updated_at timestamp with time zone default now() not null,
  full_name text not null,
  email text unique not null,
  employee_role text,
  password text not null
);

-- opcional: trigger para atualizar automaticamente o campo updatedAt
create or replace function update_updatedAt_column()
returns trigger as $$
begin
  new."updatedAt" = now();
  return new;
end;
$$ language plpgsql;

create trigger set_updatedAt
before update on USERS
for each row
execute procedure update_updatedAt_column();
