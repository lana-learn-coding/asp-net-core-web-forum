<template>
  <crud-table
    :table="table"
    :form="form"
    title="Users"
    url="admin/users"
    :filter="filter"
    form-width="400px"
  >
    <template #filter="{bind, on}">
      <v-text-field
        class="mr-3"
        v-debounce="on.search"
        :value="bind.search"
        append-icon="search"
        label="Search"
        single-line
        hide-details
      >
      </v-text-field>
      <v-spacer class="d-none d-md-block"></v-spacer>
    </template>

    <template #form="{values, inputs, errors}">
      <v-row>
        <v-col cols="12">
          <v-text-field
            :value="values.username"
            :error-messages="errors.username"
            @input="inputs.username"
            label="Username"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>

        <v-col cols="12">
          <v-text-field
            :value="values.password"
            :error-messages="errors.password"
            @input="inputs.password"
            label="Password"
            type="password"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>

        <v-col cols="12">
          <v-text-field
            :value="values.email"
            :error-messages="errors.email"
            @input="inputs.email"
            label="Email"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>

        <v-col cols="12">
          <auto-complete-select
            label="Role"
            uri="admin/users/roles/all"
            item-text="name"
            :value="values.roleIds"
            @input="inputs.roleIds"
            :error-messages="errors.roleIds"
            item-value="uid"
            persistent-placeholder
            multiple
          >
          </auto-complete-select>
        </v-col>
      </v-row>
    </template>

    <template #table.avatar="{ item }">
      <v-avatar size="32">
        <v-img
          :src="item.avatar || '@/assets/anon_.png'"
          lazy-src="@/assets/anon_thumbnail.png">
        </v-img>
      </v-avatar>
    </template>

    <template #table.emailConfirmToken="{ item }">
      <v-chip color="success" dark label small v-if="!item.emailConfirmToken">Confirmed</v-chip>
      <v-chip color="error" dark label small v-else>Not Confirmed</v-chip>
    </template>

    <template #table.roles="{ item }">
      <template v-if="item.roles.length">
        <v-chip color="primary" dark label small v-for="role of item.roles" :key="role.name">
          {{ role.name }}
        </v-chip>
      </template>
      <v-chip v-else label small>User</v-chip>
    </template>

    <template #table.updatedAt="{ item }">
      {{ formatDateTime(item.updatedAt) }}
    </template>
  </crud-table>
</template>

<script lang="ts">
import { defineComponent, reactive } from '@vue/composition-api';
import { formatDateTime } from '@/composable/date';
import CrudTable from '@/components/CrudTable.vue';
import CrudEditForm from '@/components/CrudEditForm.vue';
import EditorInput from '@/components/form/EditorInput.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'ManageUser',
  components: {
    AutoCompleteSelect,
    EditorInput,
    CrudEditForm,
    CrudTable,
  },
  setup() {
    const table = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'Avatar', value: 'avatar' },
      { text: 'Username', value: 'username' },
      { text: 'Email', value: 'email' },
      { text: 'Threads', value: 'threadsCount' },
      { text: 'Email Confirm', value: 'emailConfirmToken' },
      { text: 'Posts', value: 'postsCount' },
      { text: 'Votes', value: 'votesCount' },
      { text: 'Roles', value: 'roles', sortable: false },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action', sortable: false },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      username: '',
      email: '',
      password: '',
      roleIds: [],
    });

    return {
      table,
      filter,
      formatDateTime,
      form,
    };
  },
});
</script>
