<template>
  <v-app>
    <v-app-bar app flat color="white">
      <v-container class="py-0 fill-height">
        <v-toolbar-title class="ml-1">Dr. Forum</v-toolbar-title>

        <v-spacer></v-spacer>

        <v-responsive>
          <v-text-field
            v-model="keyword"
            dense
            flat
            clearable
            hide-details
            @focus="focus = true"
            @blur="focus = false"
            :solo="focus"
            :solo-inverted="!focus"
            prepend-inner-icon="search"
          >
          </v-text-field>
        </v-responsive>

        <v-spacer></v-spacer>

        <template v-for="link in navs">
          <v-menu
            v-if="link.links"
            :key="link.text"
            offset-y
            open-on-hover
            close-on-click
            bottom
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                class="mr-2"
                v-bind="attrs"
                v-on="on"
                text
              >
                {{ link.text }}
              </v-btn>
            </template>
            <v-list>
              <v-list-item
                v-for="(item) in link.links"
                :key="item.text"
              >
                <v-list-item-title>{{ item.text }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>

          <v-btn
            v-else
            class="mr-2"
            :key="link.text"
            :to="{ name: link.name }"
            text
            exact-path
          >
            {{ link.text }}
          </v-btn>
        </template>

        <v-btn
          v-if="!user.isAuthenticated"
          class="mr-3"
          :to="{ name: 'Login' }"
          exact-path
          text
        >
          Login
        </v-btn>

        <div v-else>
          <v-menu
            bottom
            min-width="400px"
            rounded
            offset-y
          >
            <template v-slot:activator="{ on }">
              <v-btn
                icon
                x-large
                v-on="on"
              >
                <v-avatar color="purple darken-1"></v-avatar>
              </v-btn>
            </template>
            <v-card>
              <v-list-item-content class="justify-center">
                <div class="mx-auto text-center">
                  <v-avatar color="purple darken-1 mb-2"></v-avatar>
                  <h3>{{ user.fullName }}</h3>
                  <p class="caption mt-1">
                    {{ user.email }}
                  </p>
                  <v-divider class="my-1"></v-divider>
                  <v-divider class="my-1"></v-divider>
                  <v-btn
                    depressed
                    dense
                    text
                    :to="{ name: 'Logout' }"
                  >
                    Log out
                  </v-btn>
                  <v-divider class="my-1"></v-divider>
                  <v-btn
                    v-can="'Admin'"
                    depressed
                    dense
                    text
                    :to="{ name: 'Admin' }"
                  >
                    Admin
                  </v-btn>
                </div>
              </v-list-item-content>
            </v-card>
          </v-menu>
        </div>
      </v-container>
    </v-app-bar>

    <v-main class="grey lighten-3">
      <v-container class="mt-2 mt-md-3 mt-lg-4 mt-xl-5">
        <router-view></router-view>
      </v-container>
    </v-main>
  </v-app>

</template>

<script lang="ts">
import { defineComponent, ref, watch } from '@vue/composition-api';
import { useUser } from '@/services/auth';
import { useCategories } from '@/composable/form';

export default defineComponent({
  name: 'Layout',
  setup() {
    const focus = ref(false);
    const navs = ref<NavLink[]>([
      {
        text: 'Forums',
        name: 'Home',
      },
      {
        text: 'Portal',
        name: 'Portal',
      },
      {
        text: 'Search',
        name: 'Search',
      },
      {
        text: 'Topics',
        links: [],
      },
    ]);

    const { user } = useUser();
    const keyword = ref('');

    const categories = useCategories();
    watch(categories.loading, (loading) => {
      if (loading) return;
      const topic = navs.value.find((nav) => nav.text === 'Topics');
      if (!topic) return;
      topic.links = categories.data.value.map((cat) => ({ text: cat.name, name: 'Categories', slug: cat.slug }));
      navs.value = [...navs.value];
      console.log(navs.value);
    });

    return {
      user,
      navs,
      keyword,
      focus,
    };
  },
});

interface NavLink {
  text: string,
  name?: string,
  links?: { text: string, name: string, slug: string }[]
}
</script>
