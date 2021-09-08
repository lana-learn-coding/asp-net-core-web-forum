<template>
  <v-card flat class="mt-4">
    <v-card-title>Yours Account</v-card-title>
    <v-card-subtitle>Manage your information</v-card-subtitle>
    <v-card-text>
      <v-row>
        <v-col cols="3" xl="2" class="d-none d-md-block">
          <v-img
            class="rounded"
            :src="user.avatar || require('@/assets/anon.png')"
            lazy-src="@/assets/anon_thumbnail.png"
            aspect-ratio="1"
            :alt="user.username"
          ></v-img>
        </v-col>
        <v-col cols="12" md="9" lg="8" xl="6">
          <v-text-field
            :value="user.username"
            label="Username"
            dense
            outlined
            persistent-placeholder
            readonly
          ></v-text-field>
          <v-text-field
            :value="user.email"
            label="Email"
            dense
            outlined
            persistent-placeholder
            readonly
          ></v-text-field>
          <me-change-password :user="user"></me-change-password>
          <me-change-avatar :user="user"></me-change-avatar>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Me } from '@/services/model';
import { useRequireAuth } from '@/services/auth';
import MeChangePassword from '@/views/user/account/MeChangePassword.vue';
import MeChangeAvatar from '@/views/user/account/MeChangeAvatar.vue';

export default defineComponent({
  name: 'MeAccount',
  components: { MeChangeAvatar, MeChangePassword },
  props: {
    user: {
      type: Object as PropType<Me>,
      required: true,
    },
  },
  setup() {
    useRequireAuth('User');
  },
});
</script>
