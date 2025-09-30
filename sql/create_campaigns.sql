create table campaigns (
  id uuid primary key default gen_random_uuid(),
  name text not null,
  client_id uuid not null,
  status text not null,
  created_at timestamp with time zone default now() not null,
  updated_at timestamp with time zone default now() not null,
  user_id uuid not null,

  -- relações (caso queira integrar com users e clients)
  constraint fk_campaigns_user foreign key (user_id) references users(id),
  constraint fk_campaigns_client foreign key (client_id) references clients(id)
);