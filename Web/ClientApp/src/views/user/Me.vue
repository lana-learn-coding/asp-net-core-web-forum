<template>
  <v-col cols="12" md="9">
    <v-card flat>
      <v-card-title>Yours Profile</v-card-title>
      <v-card-subtitle>Manage your information</v-card-subtitle>
      <v-card-text>

      </v-card-text>
    </v-card>

    <me-account :user="user"></me-account>
  </v-col>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import MeAccount from '@/views/user/account/MeAccount.vue';
import { useUser } from '@/services/auth';

export default defineComponent({
  name: 'Me',
  components: { MeAccount },
  setup() {
    useTitle('Your account');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Me', disabled: true },
    ]);
    const { user } = useUser();
    return {
      user,
    };
  },
});
</script>
